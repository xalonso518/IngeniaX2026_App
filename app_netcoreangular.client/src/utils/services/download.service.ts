
import { Injectable } from '@angular/core'
import { HttpClient, HttpResponse } from '@angular/common/http'
import { Observable } from 'rxjs'

@Injectable({ providedIn: 'root' })
export class DownloadService {
  constructor(private http: HttpClient) {}

  downloadFile(url: string, fallbackFilename = 'download'): void {
    this.http.get(url, {
      responseType: 'blob',
      observe: 'response'
    }).subscribe({
      next: (response: HttpResponse<Blob>) => {
        const blob = response.body!
        const filename = this.getFilename(response, fallbackFilename)
        this.saveBlob(blob, filename)
      },
      error: (err) => console.error('Download failed:', err)
    })
  }

  private getFilename(response: HttpResponse<Blob>, fallback: string): string {
    const disposition = response.headers.get('Content-Disposition')
    if (disposition) {
      const match = disposition.match(/filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/)
      if (match?.[1]) {
        return match[1].replace(/['"]/g, '')
      }
    }
    return fallback
  }

  private saveBlob(blob: Blob, filename: string): void {
    const url = URL.createObjectURL(blob)
    const anchor = document.createElement('a')
    anchor.href = url
    anchor.download = filename
    anchor.click()
    URL.revokeObjectURL(url)
  }
}

//   downloadPDF(): void {
//     this.download.downloadFile('/api/reports/monthly.pdf', 'monthly-report.pdf')
//   }
