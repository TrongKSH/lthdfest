import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from './shared/header/header.component';
import { LangSwitchComponent } from './shared/lang-switch/lang-switch.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, HeaderComponent, LangSwitchComponent],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('frontend');
}
