import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { BackendService } from '../services/backend.service';
import { NgFor } from '@angular/common';
import { takeUntil, switchMap, Subject } from 'rxjs';
import { StutEventSystem } from '../services/stut-event-system';

@Component({
  selector: 'app-diaphragm',
  standalone: true,
  imports: [NgFor],
  templateUrl: './diaphragm.component.html',
  styleUrl: './diaphragm.component.css'
})
export class DiaphragmComponent implements OnInit, OnDestroy {
  exercises?: Array<Array<string>> = [];
  private unsubscribe$ = new Subject<void>();

constructor(
   private eventSystem: StutEventSystem) {
}

  ngOnInit(): void {

    this.eventSystem.trigger$.pipe(
      takeUntil(
      this.unsubscribe$),
    ).subscribe(data =>{
      let strings: string[] = [];

      for (let i = 1; i <= 20; i++) {
        strings.push(`${data.value}, ${data.value}, ${data.value}`);
    }

      this.exercises?.push(strings) })
  }

      //TODO вынести в базовый класс
      ngOnDestroy(): void {
        this.unsubscribe$.next()
      }
    
}

