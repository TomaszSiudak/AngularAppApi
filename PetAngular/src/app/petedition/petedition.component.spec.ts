/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PeteditionComponent } from './petedition.component';

describe('PeteditionComponent', () => {
  let component: PeteditionComponent;
  let fixture: ComponentFixture<PeteditionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PeteditionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PeteditionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
