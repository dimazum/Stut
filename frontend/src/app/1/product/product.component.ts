import { Component } from '@angular/core';
import { NgIf, UpperCasePipe, NgFor } from "@angular/common";
import {FormsModule} from '@angular/forms'

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [NgIf, UpperCasePipe, NgFor, FormsModule],
  templateUrl: './product.component.html',
  styleUrl: './product.component.css'
})
export class ProductComponent {

  data = ['dsd', 'dasd', 'dqwdqw'];
  isEnabled = true;

  name2 = '';


  click(){
    this.isEnabled = !this.isEnabled
    console.log("Hi!!!")
  }

  changeText(){



    console.log("fff");
    console.log(this.name2);
  }
}


