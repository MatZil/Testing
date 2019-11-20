import { Component, OnInit } from '@angular/core';
import { FilesService } from '..//..//services/files.service';
import { FileType } from '../../helpers/file-type';

@Component({
  selector: 'app-pdf',
  templateUrl: './pdf.component.html',
  styleUrls: ['./pdf.component.scss']
})
export class PdfComponent implements OnInit {

  page = 1;
  pdfSrc = '';
  showPdf = false;
  constructor(private fileService: FilesService) { }

  ngOnInit() {
  }

  onButtonClick() {
    this.fileService.getFilePathByType(FileType.holidayPolicy).subscribe(
      data => {
        this.pdfSrc = `https://localhost:44374/${data}`;
      });
    console.log(this.pdfSrc);
    this.showPdf = !this.showPdf;
    const img: any = document.querySelector(this.pdfSrc);

    if (typeof (FileReader) !== 'undefined') {
      const reader = new FileReader();

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
