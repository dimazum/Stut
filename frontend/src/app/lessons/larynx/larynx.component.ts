import { Component } from '@angular/core';
import { BackendService } from '../../services/backend.service';
import { Observable } from 'rxjs';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-larynx',
  standalone: true,
  imports: [AsyncPipe],
  templateUrl: './larynx.component.html',
  styleUrl: './larynx.component.css'
})
export class LarynxComponent {

  wordA: string = ''
  wordO: string = ''
  wordU: string = ''
  wordE: string = ''

  constructor(private bs: BackendService){
    this.bs.getRandomWord("a", 4).subscribe(x => this.wordA = x);
    this.bs.getRandomWord("о", 4).subscribe(x => this.wordA = x); 
    this.bs.getRandomWord("у", 4).subscribe(x => this.wordA = x); 
    this.bs.getRandomWord("э", 4).subscribe(x => this.wordA = x); 
  }
}
