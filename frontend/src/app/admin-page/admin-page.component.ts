import { Component, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';

@Component({
  selector: 'app-admin-page',
  standalone: true,
  imports: [],
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {

  public version: string | undefined = undefined;

  constructor(private backendService: BackendService) {
  }

  public ngOnInit(): void {
    this.backendService.getCurrentVersion().subscribe(data => (this.version = data));
  }

}
