import { Component } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'stu-larynx',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './larynx.component.html',
  styleUrl: './larynx.component.css',
})
export class LarynxComponent {
  public wordA: string = '';
  public wordO: string = '';
  public wordU: string = '';
  public wordE: string = '';

  public constructor(private bs: BackendService) {
    this.bs.getRandomWord('a', 4).subscribe(x => (this.wordA = x));
    this.bs.getRandomWord('о', 4).subscribe(x => (this.wordA = x));
    this.bs.getRandomWord('у', 4).subscribe(x => (this.wordA = x));
    this.bs.getRandomWord('э', 4).subscribe(x => (this.wordA = x));
  }
}
