import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import { ConstantsCatalogos } from '../../../utils/constants/ConstantsCatalogos';
import { ErrorValidationService } from '../../../utils/services/error-validation.service';
import { FileInput } from '../../../models/file/FileInput';

@Component({
  selector: 'app-file-input',
  imports: [],
  templateUrl: './file-input.component.html',
  styleUrl: './file-input.component.css',
})
export class FileInputComponent {

  @Input() imagen: string = "";
  @Input() name: string = "";
  @Input() ext: string[] = [];
  @Input() showImage: boolean = false;
  @Input() placeholder: string = "";
  @Input() reset: boolean = false;
  @Output() onChangeFile = new EventEmitter<FileInput>();
  fileInput: FileInput = new FileInput('', '', '');
  //urlPublic = ConstantsCatalogos.URL_PUBLIC;
  constructor(
    private errorValidationService: ErrorValidationService,
    public _DomSanitizationService: DomSanitizer
  ) {

  }

  //onChange(event: any, name: string, ext?: any[]) {
  //  var files = event.srcElement.files;
  //  this.setFileInput(files[0], name, ext);
  //}

  onChange(event: any, name: string, ext?: any[]) {
    var files = event.srcElement.files;

    for (var i = 0; i < files.length; i++) {
      this.setFileInput(files[i], name, ext);
    }
  }

  drop(event: any, name: string, ext?: any[]) {
    event.preventDefault();
    if (event.dataTransfer.items) {
      for (var i = 0; i < event.dataTransfer.items.length; i++) {
        if (event.dataTransfer.items[i].kind === 'file') {
          this.setFileInput(event.dataTransfer.items[i].getAsFile(), name, ext);
        }
      }
    }
  }

  dragover(event: any) {
    event.preventDefault();
    event.srcElement.classList.add('file-drop-zone-drop');
  }

  dragleave(event: any) {
    event.srcElement.classList.remove('file-drop-zone-drop');
  }

  setFileInput(file: any, name: string, ext?: any[]) {
    if (file != undefined) {

      // if (ext != null && ext != undefined && ext.length > 0) {
      //   ext.forEach(e => console.log(" ext " + e));
      // }

      this.reset = false;
      if (ext && !this.errorValidationService.isValidSomeFileExtension(file, ext)) {
        alert("eerr");
      } else {
        if (file) {
          if (name == "NA") name = this.errorValidationService.getFileName(file);
          this.fileInput = new FileInput(file, name, window.URL.createObjectURL(file));
          this.onChangeFile.emit(this.fileInput);
        }
      }
    }
  }
}
