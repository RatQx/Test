import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss'],
})
export class InformationComponent {
  selectedSection: string | null = null;
  constructor() {}

  toggleSection(section: string) {
    this.selectedSection = this.selectedSection === section ? null : section;
  }
}
