import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {PdfViewerDialogData} from './pdf-viewer-dialog-data';

@Component({
  selector: 'app-pdf-viewer-dialog',
  templateUrl: './pdf-viewer-dialog.component.html',
  styleUrls: ['./pdf-viewer-dialog.component.scss']
})
export class PdfViewerDialogComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: PdfViewerDialogData,
              private dialog: MatDialogRef<PdfViewerDialogComponent>) { }

  ngOnInit(): void {
  }

  closeDialog() {
    this.dialog.close();
  }

}
