import { Component } from '@angular/core';
import { TriggerBlockComponent } from "../trigger-block/trigger-block.component";
import { RandomTextComponent } from "../random-text/random-text.component";
import { CommonModule } from '@angular/common';

export const LESSON_TYPE = {
  NONE: 'none',
  READ: 'read',
  JUST_REPEAT: 'justRepeat',
  NO_VOICE: 'noVoice',
} as const;

export type LessonType =
  typeof LESSON_TYPE[keyof typeof LESSON_TYPE];


@Component({
  selector: 'stu-daily-lesson',
  standalone: true,
  imports: [TriggerBlockComponent, RandomTextComponent, CommonModule],
  templateUrl: './daily-lesson.component.html',
  styleUrl: './daily-lesson.component.css'
})
export class DailyLessonComponent {

  readonly LESSON_TYPE = LESSON_TYPE;
  currentLessonType: LessonType = LESSON_TYPE.NONE

  public setLessonType(type: LessonType){
    this.currentLessonType = type;
  }

}
