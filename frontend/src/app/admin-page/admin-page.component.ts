import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'stu-admin-page',
  standalone: true,
  imports: [],
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css'],
})
export class AdminPageComponent implements OnInit {
  public hash: string | undefined = undefined;

  public constructor(private backendService: BackendService) {}

  public ngOnInit(): void {
    this.backendService.getCurrentCommitHash().subscribe(data => (this.hash = data));
  }
}
