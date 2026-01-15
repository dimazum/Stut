import { Component } from '@angular/core';
import { TriggerBlockComponent } from "../trigger-block/trigger-block.component";
import { RandomTextComponent } from "../random-text/random-text.component";

@Component({
  selector: 'stu-daily-lesson',
  standalone: true,
  imports: [TriggerBlockComponent, RandomTextComponent],
  templateUrl: './daily-lesson.component.html',
  styleUrl: './daily-lesson.component.css'
})
export class DailyLessonComponent {

}
