import { Injectable } from '@angular/core';
import { LocalizationDto } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LocalizationCacheService {
  private cacheKey = 'app.localization';

  public get(): { version: string; data: LocalizationDto } | null {
    const raw = localStorage.getItem(this.cacheKey);

    return raw ? JSON.parse(raw) : null;
  }

  public set(version: string, data: LocalizationDto): void {
    localStorage.setItem(this.cacheKey, JSON.stringify({ version, data }));
  }

  public getVersion(): string | null {
    return this.get()?.version ?? null;
  }

  public getTranslations(): LocalizationDto | null {
    return this.get()?.data ?? null;
  }

  public clear(): void {
    localStorage.removeItem(this.cacheKey);
  }
}
