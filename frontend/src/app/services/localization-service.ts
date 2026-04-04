import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';

export type Translations = Record<string, string>;

export interface LocalizationDto {
  translations: Translations;
  lang: string;
}

export function initLocalization(loc: LocalizationService) {
  return () => loc.init();
}

@Injectable({ providedIn: 'root' })
export class LocalizationService {
  public constructor(
    private http: HttpClient,
    private translate: TranslateService
  ) {}

  public async init(): Promise<void> {
    this.translate.setFallbackLang('en');

    const dto = await firstValueFrom(
      this.http.get<LocalizationDto>('/api/localization')
    );

    this.translate.setTranslation(dto.lang, dto.translations);
    this.translate.use(dto.lang);
  }
}
