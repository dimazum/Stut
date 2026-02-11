import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-warm-up',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './warm-up.component.html',
  styleUrl: './warm-up.component.css'
})
export class WarmUpComponent {

data = [
  { char: '', air: 40 },
  { char: '', air: 70 },
  { char: '', air: 65 },
  { char: 'п', air: 60 },
  { char: 'р', air: 58 },
  { char: 'и', air: 56 },
  { char: 'в', air: 54 },
  { char: 'е', air: 52 },
  { char: 'т', air: 50 },
  { char: ',', air: 46 },
  { char: ' ', air: 42 },
  { char: 'м', air: 40 },
  { char: 'е', air: 38 },
  { char: 'н', air: 36 },
  { char: 'я', air: 34 },
  { char: ' ', air: 32 },
  { char: 'з', air: 30 },
  { char: 'о', air: 28 },
  { char: 'в', air: 26 },
  { char: 'у', air: 24 },
  { char: 'т', air: 22 },
  { char: ' ', air: 20 },
  { char: 'В', air: 18 },
  { char: 'и', air: 16 },
  { char: 'к', air: 14 },
  { char: 'а', air: 12 },
  { char: '.', air: 6 },
  { char: ' ', air: 2 },


  
];

splitHeight(current: number, prev: number, part: number){

  if(part === 0){
    return prev + (current - prev) * .25
  }

   if(part === 1){
    return prev + (current - prev) * .5
  }

   if(part === 2){
    return prev + (current - prev) * .75
  }

    if(part === 3){
    return current
  }

  return 0
}

}
