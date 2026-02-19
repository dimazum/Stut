import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Observable, Subscription } from 'rxjs';
import { VoiceAnalysisUpdateDto } from '../models/models';
import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class VoiceAnalysisService implements OnDestroy {

  private hubConnection?: signalR.HubConnection;
  private authSubscription: Subscription;

  private voiceAnalysisSubject = new BehaviorSubject<VoiceAnalysisUpdateDto | null>(null);
  public voiceAnalysis$: Observable<VoiceAnalysisUpdateDto | null> =
    this.voiceAnalysisSubject.asObservable();

  constructor(private authService: AuthService) {

    this.authSubscription = this.authService.userinfo$.subscribe(user => {
      if (user?.logged_in) {
        this.startConnection();
      } else {
        this.stopConnection();
      }
    });
  }

  private async startConnection(): Promise<void> {

    if (this.hubConnection) {
      return;
    }

    const url = environment.baseUrl + '/voice-analysis';

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: () => localStorage.getItem('jwt-token') || ''
      })
      .withAutomaticReconnect()
      .build();

    //Получение результата от сервера
    this.hubConnection.on('UpdateVoiceAnalysis',
      (data: VoiceAnalysisUpdateDto) => {
        this.voiceAnalysisSubject.next(data);
      }
    );

    try {
      await this.hubConnection.start();
      console.log('✅ Connected to VoiceAnalysisHub');
    } catch (err) {
      console.error('❌ SignalR connection error:', err);
    }
  }


  // SEND STRING TO BACKEND
  public async analyzeVoice(text: string): Promise<void> {

    if (!this.hubConnection) {
      console.warn('SignalR not connected');
      return;
    }

    try {
      await this.hubConnection.invoke('AnalyzeVoice', text);
    } catch (err) {
      console.error('Error sending text to hub:', err);
    }
  }

  private async stopConnection(): Promise<void> {

    if (!this.hubConnection) {
      return;
    }

    try {
      await this.hubConnection.stop();
      console.log('SignalR disconnected');
    } catch (err) {
      console.error(err);
    }

    this.hubConnection = undefined;
    this.voiceAnalysisSubject.next(null);
  }

  ngOnDestroy(): void {
    this.stopConnection();
    this.authSubscription.unsubscribe();
    this.voiceAnalysisSubject.complete();
  }
}
