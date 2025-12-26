import { Component, Input, OnChanges } from '@angular/core';
import { ChartOptions, ChartDataset } from 'chart.js';
import { NgChartsModule } from 'ng2-charts';
import { VoiceAnalysisResult } from '../../models/models';

@Component({
  selector: 'app-voice-diagrams',
  standalone: true,
  imports: [NgChartsModule],
  templateUrl: './voice-diagrams.component.html',
  styleUrls: ['./voice-diagrams.component.css']
})
export class VoiceDiagramsComponent implements OnChanges {
  @Input() results: VoiceAnalysisResult[] = [];

  public lineChartData: { labels: string[], datasets: ChartDataset<'line'>[] } = {
    labels: [],
    datasets: []
  };

  public lineChartOptions: ChartOptions = { responsive: true };
  public lineChartLegend = true;

  ngOnChanges(): void {
  if (!this.results || this.results.length === 0) return;

  const labels = this.results.map(r =>
    new Date(r.recordedAt).toLocaleDateString()
  );

  const datasets: ChartDataset<'line'>[] = [
    {
      data: this.results.map(r => r.pitchMean ?? 0),
      label: 'Pitch Mean',
      borderColor: 'blue',
      backgroundColor: 'rgba(0,0,255,0.1)',
      tension: 0.3
    },
    {
      data: this.results.map(r => r.volumeMeanDb),
      label: 'Volume Mean',
      borderColor: 'green',
      backgroundColor: 'rgba(0,255,0,0.1)',
      tension: 0.3
    },
    {
      data: this.results.map(r => r.speechRate),
      label: 'Speech Rate',
      borderColor: 'red',
      backgroundColor: 'rgba(255,0,0,0.1)',
      tension: 0.3
    },
    {
      data: this.results.map(r => r.pauseRatio),
      label: 'Pause Ratio',
      borderColor: 'orange',
      backgroundColor: 'rgba(255,165,0,0.1)',
      tension: 0.3
    }
  ];

  this.lineChartData = {
    labels,
    datasets
  };
}

}
