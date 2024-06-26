#include "StepperController.h"

void StepperController::StartRunClockwise()
{
    digitalWrite(currentEnablePin, LOW);
    this->setCurrentPosition(0);
    this->setSpeed(sps);
    this->isRunning = true;
    this->isRunningClockwise = true;
    this->runSpeed();
}

void StepperController::StartRunAnticlockwise()
{
    digitalWrite(currentEnablePin, LOW);
    this->setCurrentPosition(0);
    this->setSpeed(-sps);
    this->isRunning = true;
    this->isRunningClockwise = false;
    this->runSpeed();
}

void StepperController::UpdateStepper()
{
    if(!this->isRunning)
        return;

    if(isRunningClockwise)
        this->setSpeed(sps);
    else
        this->setSpeed(-sps);
    
    this->runSpeed();
    if(this->currentPosition() == this->steps90DegreesRotation ||
    this->currentPosition() == -this->steps90DegreesRotation)
    {
        this->stop();
        digitalWrite(this->currentEnablePin, HIGH);
        this->isRunning = false; 
    }
}

bool StepperController::IsRunning()
{
    return this->isRunning;
}

void StepperController::setCurrentEnablePin(uint8_t pin)
{
    this->currentEnablePin = pin;
}