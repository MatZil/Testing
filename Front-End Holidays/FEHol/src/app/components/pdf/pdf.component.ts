import { Component, OnInit } from '@angular/core';
import { FilesService } from '..//..//services/files.service';
import { FileType } from '../../helpers/file-type';
import { environment } from '../../../environments/environment';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-pdf',
  templateUrl: './pdf.component.html',
  styleUrls: ['./pdf.component.scss']
})
export class PdfComponent implements OnInit {

  page = 1;
  pdfSrc = '';
  showPdf = false;
  fileTypes = new FileType();
  constructor(private fileService: FilesService, private alertService: AlertService) { }

  ngOnInit() {
  }

  onButtonClick() {
    this.fileService.getFilePathByType(this.fileTypes.holidayPolicy).subscribe(
      data => {
        this.pdfSrc = `${environment.serverUrl}/${data}`;
      }
    );

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
