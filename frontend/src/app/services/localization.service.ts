import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';
import { LocalizationCacheService } from './localization-cache.service';
import { LocalizationDto } from '../models/models';

export type Translations = Record<string, string>;

export function initLocalization(loc: LocalizationService) {
  return () => loc.init();
}

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  public constructor(
    private http: HttpClient,
    private translate: TranslateService,
    private localizationCacheService: LocalizationCacheService
  ) {}

  public async init(): Promise<void> {
    this.translate.setFallbackLang('en');

    let localization: LocalizationDto = this.localizationCacheService.get();

    if (localization) {
      this.applyLocalization(localization);

      return;
    }

    localization = await firstValueFrom(this.http.get<LocalizationDto>('/api/localization'));
    this.localizationCacheService.set(localization);
    this.applyLocalization(localization);
  }

  private applyLocalization(localization: LocalizationDto): void {
    this.translate.setTranslation(localization.lang, localization.translations);
    this.translate.use(localization.lang);
  }
}
