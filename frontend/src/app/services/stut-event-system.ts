import { Injectable, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { Trigger } from '../models/models';

@Injectable({
  providedIn: 'root',
})
export class StutEventSystem implements OnDestroy {
  protected unsubscribe$ = new Subject<void>();

  public trigger$ = new Subject<Trigger>();

  public ngOnDestroy(): void {
    this.unsubscribe$.next();
  }
}
