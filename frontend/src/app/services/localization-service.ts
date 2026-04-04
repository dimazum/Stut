import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TranslateService } from '@ngx-translate/core';
import { firstValueFrom } from 'rxjs';

export type Translations = Record<string, string>;

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
    const lang = localStorage.getItem('lang') ?? this.getBrowserLang() ?? 'en';

    this.translate.setFallbackLang('en');

    const translations = await firstValueFrom(
      this.http.get<Translations>('/api/localization', { params: { culture: lang } })
    );

    this.translate.setTranslation(lang, translations);
    this.translate.use(lang);
  }

  public async changeLanguage(lang: string) {
    const translations = await firstValueFrom(
      this.http.get<Translations>('/api/localization', { params: { culture: lang } })
    );

    this.translate.setTranslation(lang, translations, true);
    this.translate.use(lang);

    localStorage.setItem('lang', lang);
  }

  private getBrowserLang(): string {
    const browser = navigator.language.toLowerCase();

    if (browser.startsWith('en')) {
      return 'en';
    }

    if (browser.startsWith('ru')) {
      return 'ru';
    }

    return 'en';
  }
}
