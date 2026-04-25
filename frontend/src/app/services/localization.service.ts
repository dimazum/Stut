import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';
import { LocalizationCacheService } from './localization-cache.service';
import { LocalizationDto } from '../models/models';
import * as signalR from '@microsoft/signalr';

export function initLocalization(loc: LocalizationService) {
  return () => loc.init();
}

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  private hubConnection?: signalR.HubConnection;
  private latestServerVersion: string | null = null;

  public constructor(
    private http: HttpClient,
    private translate: TranslateService,
    private cache: LocalizationCacheService
  ) {}

  public async init(): Promise<void> {
    this.translate.setFallbackLang('en');

    this.startConnection();

    const cached = this.cache.get();

    if (cached) {
      this.applyLocalization(cached.data);
    }

    await this.ensureFreshTranslations();
  }

  private async ensureFreshTranslations(): Promise<void> {
    const cachedVersion = this.cache.getVersion();

    if (this.latestServerVersion && this.latestServerVersion !== cachedVersion) {
      await this.reloadTranslations();

      return;
    }

    if (!cachedVersion) {
      await this.reloadTranslations();
    }
  }

  private async reloadTranslations(): Promise<void> {
    const localization = await firstValueFrom(this.http.get<LocalizationDto>('/api/localization'));

    const version = localization.version;

    this.cache.set(version, localization);
    this.applyLocalization(localization);
  }

  private applyLocalization(localization: LocalizationDto): void {
    this.translate.setTranslation(localization.lang, localization.translations);
    this.translate.use(localization.lang);
  }

  private startConnection(): void {
    this.hubConnection = new signalR.HubConnectionBuilder().withUrl('/translations').withAutomaticReconnect().build();

    this.hubConnection.start().catch(err => console.error('SignalR error:', err));

    this.hubConnection.on('translationsUpdated', (data: any) => {
      console.log('Translations updated', data);

      debugger;

      this.latestServerVersion = data.version;

      const currentVersion = this.cache.getVersion();

      if (currentVersion !== data.version) {
        this.reloadTranslations();
      }
    });
  }
}
