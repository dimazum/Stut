import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'angular-content',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './main-content.component.html',
})
export class MainContentComponent {

onActivate() {
  this.showSpa();
}

onDeactivate() {
  this.showServer();
}

private showSpa() {
  document.getElementById('server-content')!.hidden = true;
  document.getElementById('spa-content')!.hidden = false;
}

private showServer() {
  document.getElementById('spa-content')!.hidden = true;
  document.getElementById('server-content')!.hidden = false;
}

}
