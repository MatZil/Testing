import {Component, OnInit} from '@angular/core';
import {FilesService} from '../../services/files.service';
import {MatDialog} from '@angular/material/dialog';
import {PdfViewerDialogComponent} from '../pdf-viewer-dialog/pdf-viewer-dialog.component';
import {PdfViewerDialogData} from '../pdf-viewer-dialog/pdf-viewer-dialog-data';

@Component({
  selector: 'app-policy',
  templateUrl: './policy.component.html',
  styleUrls: ['./policy.component.scss']
})
export class PolicyComponent implements OnInit {

  constructor(private fileService: FilesService,
              private dialog: MatDialog) {
  }

  ngOnInit() {
  }

  onButtonClick() {
    this.fileService.downloadPolicy().subscribe(blob => {
      const fileURL = URL.createObjectURL(blob);
      const data: PdfViewerDialogData = {url: fileURL, title: 'Holiday Policy'};
      this.dialog.open(PdfViewerDialogComponent, {data: data});
    });
  }
}
