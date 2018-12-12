import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Marker, Circle, LatLngLiteral, LatLngBounds } from '@agm/core/services/google-maps-types';
import { MouseEvent, AgmCircle } from '@agm/core';
import { AlertifyService } from '../../_services/alertify.service';
import { GmapParameter } from '../../_models/GmapParameter';
import { from as fromPromise } from 'rxjs';

@Component({
  selector: 'app-gmap-selector',
  templateUrl: './gmap-selector.component.html',
  styleUrls: ['./gmap-selector.component.css']
})

export class GmapSelectorComponent implements OnInit {
  @Input() lat: number;
  @Input() lng: number;
  @Input() radius: number;
  @Input() useCircle: boolean;
  @Input() radiusMax: number = 5000;
  @Input() getBoundary: boolean = false;
  @ViewChild(AgmCircle) circle;

  @Output() areaSelectedEvent = new EventEmitter<GmapParameter>();

  constructor(private alertify: AlertifyService) { }

  ngOnInit() {
    //Default position: Britomart
    if (!this.lat)
      this.lat = -36.8441;
    if (!this.lng)
      this.lng = 174.7678;
    if (!this.radius)
      this.radius = 1000;
  }

  mapClicked($event: MouseEvent) {
    this.lng = $event.coords.lng;
    this.lat = $event.coords.lat;
    this.areaChanged();
  }

  radiusChange($event: number) {
    if ($event > this.radiusMax) {
      this.alertify.error("選択範囲が広すぎます");
      this.radius = this.radiusMax;
      return;
    }
    this.areaChanged();
  }

  markerDragEnd($event: MouseEvent) {
    this.lng = $event.coords.lng;
    this.lat = $event.coords.lat;
    this.areaChanged();
  }

  centerChanged($event: LatLngLiteral) {
    this.lng = $event.lng;
    this.lat = $event.lat;
    this.areaChanged();
  }

  private areaChanged() {
    if (this.useCircle) {
      fromPromise(this.circle.getBounds()).subscribe((bounds: LatLngBounds) => {
        let ne = bounds.getNorthEast();
        let sw = bounds.getSouthWest();
        this.areaSelectedEvent.emit({
          lat: this.lat,
          lng: this.lng,
          radius: this.radius,
          area_top: ne.lat(),
          area_left: ne.lng(),
          area_bottom: sw.lat(),
          area_right: sw.lng()
        });
      })


    }
    else
      this.areaSelectedEvent.emit({ lat: this.lat, lng: this.lng, radius: null });
  }

}
