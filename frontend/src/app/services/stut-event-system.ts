import { EventEmitter, Injectable, OnDestroy, OnInit } from "@angular/core";
import { firstValueFrom, Observable, Subject, takeUntil } from 'rxjs';
import { Trigger } from "../models/models";

@Injectable({
    providedIn: 'root'
  })
export class StutEventSystem implements OnInit, OnDestroy {

    protected unsubscribe$ = new Subject<void>();

    public trigger$ = new Subject<Trigger>();

    ngOnInit(): void {

    }

    
    ngOnDestroy(): void {
        this.unsubscribe$.next();
    }

}
