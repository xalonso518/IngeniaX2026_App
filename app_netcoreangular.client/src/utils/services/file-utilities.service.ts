import { Injectable } from '@angular/core';
// import FileSaver, { saveAs } from 'file-saver';

@Injectable()
export class FileUtilitiesService {
    constructor() {

    }

    // downLoadFile(url: string, name: string = "_parent") {
        
    //     if (name == "new") {
    //         var win = window.open(url, url, "width=200,height=100");
    //         setTimeout(() => {
    //             if (win != null) win.close();
    //         }, 1000);
    //         //if (win != null) win.close();
    //     } else { var win = window.open(url, name);}
    // }

    // downLoadFileBasse64(fileBase64: string, name:string, ext: string)
    // {
    //   if (ext == "zip") {
    //     var file = new Blob([this.convertDataURIToBinary(fileBase64)], { type: 'application/zip' });
    //     FileSaver.saveAs(file, name + new Date().getTime() + '.' + ext);
    //   } else {
    //     var file = new Blob([this.convertDataURIToBinary(fileBase64)], { type: 'application/pdf' });
    //     FileSaver.saveAs(file, name + new Date().getTime() + '.' + ext);
    //   }
    // }

    // getFileBasse64(fileBase64: string, ext: string): string {
    //     var file = new Blob([this.convertDataURIToBinary(fileBase64)], { type: 'application/' + ext });
    //     var fileURL = URL.createObjectURL(file);
    //     return fileURL;
    // }

    // convertDataURIToBinary(dataURI: string) {
    //     var raw = window.atob(dataURI);
    //     var rawLength = raw.length;
    //     var array = new Uint8Array(new ArrayBuffer(rawLength));

    //     for (var i = 0; i < rawLength; i++) {
    //         array[i] = raw.charCodeAt(i);
    //     }
    //     return array;
    // }  

}
