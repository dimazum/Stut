import { Injectable, OnDestroy, OnInit } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { VoiceAnalysisUpdateDto } from '../models/models';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class VoiceAnalysisService implements OnInit, OnDestroy {
  private hubConnection?: signalR.HubConnection;
  public subscription!: Subscription;
  private signalRConnected: boolean = false;

  private voiceAnalysisSubject = new BehaviorSubject<VoiceAnalysisUpdateDto | null>(null);
  public voiceAnalysis$: Observable<VoiceAnalysisUpdateDto | null> = this.voiceAnalysisSubject.asObservable();

  constructor(private authServeice: AuthService) {
      this.subscription = this.authServeice.userinfo$.subscribe(x => 
      {
        if(x?.logged_in && !this.signalRConnected){
          const url = environment.baseUrl + '/voice-analysis' ;
          this.hubConnection = new signalR.HubConnectionBuilder()
          .withUrl(url, {
            accessTokenFactory: () => {
              return localStorage.getItem('jwt-token') || '';
            }
          })
          .withAutomaticReconnect()
          .build();

          this.hubConnection.start()
          .then(() => console.log('Connected to VoiceAnalysisHub'))
          .catch((err) => console.error(err));

          this.hubConnection.on('UpdateVoiceAnalysis', (data: VoiceAnalysisUpdateDto) => {
           this.voiceAnalysisSubject.next(data);
          });

          this.signalRConnected = true;
        }
      }
    )
  }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {
    this.hubConnection?.stop().catch(err => console.error(err));
    this.voiceAnalysisSubject.complete();
  }
}
