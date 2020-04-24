import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { AuditLog } from 'src/app/models/audit-log';
import { MatTableDataSource } from '@angular/material/table';
import { AuditLogService } from 'src/app/services/audit-log.service';

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
  dataSource = new MatTableDataSource<AuditLog>();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  
  constructor(
    private auditLogService: AuditLogService
  ) { }

  ngOnInit() {
    this.refreshTable();
    this.dataSource.paginator = this.paginator;
  }

  refreshTable(): void {
    console.log("krepiamasi");
    this.auditLogService.getAuditLogsPage(5, 5).subscribe(logs => {
      this.dataSource.data = logs;
    });
    console.log("baigesi");
    console.log(this.dataSource.data);
  }

}
