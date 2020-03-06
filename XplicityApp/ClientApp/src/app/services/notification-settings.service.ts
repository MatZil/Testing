import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { NotificationSettings } from '../models/notification-settings';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificationSettingsService {

  private readonly notificationSettingsApi = `${this.baseUrl}api/NotificationSettings`;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getNotificationSettings(userId: number): Observable<NotificationSettings> {
    return this.http.get<NotificationSettings>(`${this.notificationSettingsApi}/${userId}`);
  }

  updateNotificationSettings(userId: number, updateNotificationSettings: NotificationSettings): Observable<boolean> {
    return this.http.put<boolean>(`${this.notificationSettingsApi}/${userId}`, updateNotificationSettings);
  }
}
