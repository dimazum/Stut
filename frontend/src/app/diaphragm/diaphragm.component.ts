import { Component, OnInit } from '@angular/core';
import { NgFor } from '@angular/common';
import { Subject } from 'rxjs';
import { StutEventSystem } from '../services/stut-event-system';
import { untilDestroyed } from '@ngneat/until-destroy';

@Component({
  selector: 'stu-diaphragm',
  standalone: true,
  imports: [NgFor],
  templateUrl: './diaphragm.component.html',
  styleUrl: './diaphragm.component.css',
})
export class DiaphragmComponent implements OnInit {
  public exercises?: Array<Array<string>> = [];
  private unsubscribe$ = new Subject<void>();

  public constructor(private eventSystem: StutEventSystem) {}

  public ngOnInit(): void {
    this.eventSystem.trigger$.pipe(untilDestroyed(this)).subscribe(data => {
      const strings: string[] = [];

      for (let i = 1; i <= 20; i++) {
        strings.push(`${data.value}, ${data.value}, ${data.value}`);
      }

      this.exercises?.push(strings);
    });
  }

  //TODO вынести в базовый класс
}
