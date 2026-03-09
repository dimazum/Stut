import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-item',
  standalone: true,
  imports: [],
  templateUrl: './nav-item.component.html',
  styleUrl: './nav-item.component.css'
})
export class NavItemComponent {
  @Input() path: string = '';
  @Input() btn:string = '';

constructor(private router: Router) {}

   goToPage() {
    this.router.navigate([this.path]);
  }
}
