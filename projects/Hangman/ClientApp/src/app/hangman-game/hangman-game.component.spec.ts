import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HangmanGameComponent } from './hangman-game.component';

describe('HangmanGameComponent', () => {
  let component: HangmanGameComponent;
  let fixture: ComponentFixture<HangmanGameComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HangmanGameComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HangmanGameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
