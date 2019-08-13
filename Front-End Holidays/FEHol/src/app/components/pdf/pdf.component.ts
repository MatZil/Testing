import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pdf',
  templateUrl: './pdf.component.html',
  styleUrls: ['./pdf.component.scss']
})
export class PdfComponent implements OnInit {

  page = 1;
  pdfSrc = '/assets/policy.pdf';
  showPdf = false;
  constructor() { }

  ngOnInit() {
  }

  onButtonClick() {
    this.showPdf = !this.showPdf;
    let img: any = document.querySelector(this.pdfSrc);

    if (typeof(FileReader) !== 'undefined') {
      let reader = new FileReader();

      reader.onload = (e: any) => {
        this.pdfSrc = e.target.result;
      };

      reader.readAsArrayBuffer(img.files[0]);
    }
  }

  exitPdf() {
    this.showPdf = false;
  }

}
