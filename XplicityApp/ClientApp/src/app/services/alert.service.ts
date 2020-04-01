import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  private readonly snackbarConfiguration: MatSnackBarConfig = { duration: 5000 };
  constructor(private snackBar: MatSnackBar) { }

  public displayMessage(message: string) {
    this.snackBar.open(message, 'OK', this.snackbarConfiguration);
  }
}
