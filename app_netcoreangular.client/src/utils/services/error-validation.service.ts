import { Injectable } from '@angular/core';


@Injectable()
// Servicio para validar campos y datos
export class ErrorValidationService {
    constructor() {

    }

    isEmail(email: string): boolean {
        var re = /^[a-zA-Z_0-9.]+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
        //return re.test(email);
        return true;
    }

    isNumber(value: any): number {
        if (Number.isNaN(parseFloat(value)) || value.toString().includes("e")) return 0.00;
        else return Number.parseFloat(value);
    }

    setNumber(input: any, decimales: number): number {
        var value = input.value;
          if (value == null) return value;
        if (value != "") {
            value = Number.parseFloat(this.isNumber(value).toFixed(decimales));
            if (input.min && input.min > value) value += value * -2;
            if (input.max && input.max < value) value = 0.00;
        }
        return value;
    }

  setNumberPrestamo(input: any, decimales: number): number {
    const num = parseFloat(input);
    if (isNaN(num) || num < 0.01) return 0;
    var  valor = parseFloat(num.toFixed(decimales));
    return valor;
    
    
  }
    setNumberEnteroString(input: any, l: number): string {
        var value = input.value;
        if (value == null) return value;
        var sValue: string = value.toString();
        value = sValue = sValue.replace(/\./g, '');
        if (value != "") {
            if (l < sValue.length) value = sValue.slice(0,l);
        }
        return value;
    }

    setNumberPositive(input: any, decimales: number, max:number): number {
        var value = input.value;
        if (value == null) return value;
        if (value != "") {
            value = Number.parseFloat(this.isNumber(value).toFixed(decimales));
            if (0 > value) value += value * -2;
            if (max < value) value = 0.00;
        }
        return value;
    }

  setNumberFixedSat(value: number, decimales: number) {
 
   
        var cien = Math.pow(10, decimales);
        return (Math.round(value * cien) / cien);
  }

  setNumberFixedSatDos(value: number, decimales: number) {
    return Number((value + 1e-9).toFixed(decimales));
  }


  //setNumberFixedSat2(value: number, decimales: number) {



  //  var cien = Math.pow(10, decimales);

  //  var resSinRed = (value * cien).toFixed(this.decimalPlaces(value));

  //  var entero = Math.round(parseFloat(resSinRed))
  //  var resultado = entero / cien;
  //  return (resultado);
  //}

  //decimalPlaces(num: number) {
  //  var match = ('' + num).match(/(?:\.(\d+))?(?:[eE]([+-]?\d+))?$/);
  //  if (!match) { return 0; }
  //  return Math.max(
  //    0,
  //    // Number of digits right of decimal point.
  //    (match[1] ? match[1].length : 0)
  //    // Adjust for scientific notation.
  //    - (match[2] ? +match[2] : 0));
  //}
    setNumberFloor(value: number, decimales: number) {
        var cien = Math.pow(10, decimales);
        return (Math.floor(value * cien) / cien);
    }
    setNumberCeil(value: number, decimales: number) {
      var cien = Math.pow(10, decimales);
      return (Math.ceil(value * cien) / cien);
    }


    setNumberRound(value: number, decimales: number) {
        var cien = Math.pow(10, decimales);
        return (Math.round(value * cien) / cien);
    }

    setNumberRound2(value: number, decimales: number) {
        var cien = Math.pow(10, decimales);
        var x = Number.parseFloat(this.isNumber(value).toFixed(decimales));
        return (x);
  }

  setNumberTrunc(value: number) {
    let t = value.toString();
    let regex = /(\d*.\d{0,6})/;
    var rRegex = t.match(regex);
    if (rRegex != null) {
      return rRegex[0];
    } else {
      return 0;
    }
  }

    getFileName(file: any): string{
      var division = file.name.split(".");
     // var name: string = division.length > 0 ? division[0] : "archivo";

      var nameConPuntos = "";

      if (division.length > 2) {
        for (var i = 0; i < division.length - 1; i++) {

          nameConPuntos += i < division.length - 2 ? division[i] + "." : division[i];
        }
      }

      var name: string = nameConPuntos != "" ? nameConPuntos : division.length > 0 ? division[0] : "archivo";
      
        return name.trim();
    }

    isValidFileSize(file: any): boolean {
        var MAX_FILE_SIZE = 1024 * 1024 * 5;
        if (file.size > MAX_FILE_SIZE) return false;
        return true;
    }

    isValidSomeFileExtension(file: any, exts: any[]): boolean {
        var division = file.name.split(".");
        var ext = division.length > 0 ? division[division.length - 1] : "txt";

      var z = exts.findIndex(e => e == ext.toLowerCase());
      // Ticket 5117 [Bookkeep Pro] Archivos adjuntos/No es posible seleccionar varios archivos
      // Se pide quitar validación de extensiones, puede ser cualquiera 28/05/2026
      // Se comentó que podría tener riesgo quitar la validación dado que  no se sabe todo su alcance en el sistema
       // if (z == -1) return false;
        return true;
    }

    isValidFileImgExtension(file: any): boolean {
        var FILE_EXTENSIONS = ["png", "jpg", "jpeg", "svg", "gif"];

        var division = file.name.split(".");
        var ext = division.length > 0 ? division[division.length - 1] : "txt";

        var z = FILE_EXTENSIONS.findIndex(e => e == ext.toLowerCase());
        if (z == -1) return false;
        return true;
    }

    isValidFileExtension(file: any): boolean {
        var FILE_EXTENSIONS = ["pdf", "xml", "rar", "zip",
            "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt",
            "png", "jpg", "jpeg", "svg", "gif", "cert", "key"];

        var division = file.name.split('.');
        var ext = division.length > 0 ? division[division.length - 1] : "txt";

        if (FILE_EXTENSIONS.findIndex(ext.toLowerCase()) == -1) return false;
        return true;
    }

    validateFile(file: any): boolean {
        if (file == null) return false;
        return this.isValidFileSize(file) && this.isValidFileExtension(file);
    }

    validateFileImg(file: any): boolean {
        if (file == null) return false;
        return this.isValidFileSize(file) && this.isValidFileImgExtension(file);
    }

    getFechaZeroString(date: Date): string {
        var month = (date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1);
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return date.getFullYear() + "-" + month + "-" + day;
    }

    getFechaEspZeroString(date: Date): string {
        var month = (date.getMonth() + 1) < 10 ? "0" + (date.getMonth() + 1) : (date.getMonth() + 1);
        var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
        return day + "/" + month + "/" + date.getFullYear();
    }

}
