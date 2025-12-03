import { Component, signal } from '@angular/core';
import { JournalComponent } from "./features/journal/journal.component";


@Component({
  selector: 'app-root',
  imports: [JournalComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class App {
  protected readonly title = signal('fCarePlusCleint');
}
