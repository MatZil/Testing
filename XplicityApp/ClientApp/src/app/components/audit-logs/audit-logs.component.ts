import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { AuditLog } from 'src/app/models/audit-log';
import { AuditLogService } from 'src/app/services/audit-log.service';
import { merge } from 'rxjs';
import { map, startWith, switchMap} from 'rxjs/operators';

@Component({
  selector: 'app-audit-logs',
  templateUrl: './audit-logs.component.html',
  styleUrls: ['./audit-logs.component.scss']
})
export class AuditLogsComponent implements OnInit {

  displayedColumns: string[] = [
    'data',
    'entityType',
    'date',
    'user'
  ];

  totalItemsCount: number;
  data: AuditLog[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;

  entityNames : string[] = [
    'Client',
    'User',
    'Employee',
    'Holiday',
    'IdentityRole',
    'InventoryItem',
    'InventoryItemTag',
    'NotificationSettings',
    'Tag',
    'EmailTemplate',
    'FileRecord',
    'IdentityUserRole`1'
  ];

  entityName = '';
  
  constructor(
    private auditLogService: AuditLogService
  ) { }

  ngOnInit() {
  }

  ngAfterViewInit() {
    this.paginator.pageIndex = 0;

    merge(this.paginator.page)
    .pipe(
      startWith({}),
      switchMap(() => {
        return this.auditLogService.getAuditLogsByType(
          this.entityName, this.paginator.pageIndex + 1, this.paginator.pageSize);
        }),
      map(data => {
        this.totalItemsCount = data.totalCount;
        return data.logs;
      }))
      .subscribe(data => {
        for (var i = 0; i < data.length; i++) {
          data[i].data = JSON.parse(data[i].data);
        }
        this.data = data;
    }); 
  }

}
