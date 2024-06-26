#pragma once
#include <AccelStepper.h>

class StepperController : public AccelStepper
{
private:
    uint8_t currentEnablePin;
    bool isRunning;
    bool isRunningClockwise; 
    float sps;   // Steps per second
    uint16_t microstepping;
    uint16_t stepsPerFullRotation;
    uint16_t steps90DegreesRotation;
public:
    StepperController() : AccelStepper() {};
    StepperController(uint8_t interface,
                    uint8_t step_pin,
                    uint8_t dir_pin,
                    uint8_t defaultStepsSpecs,
                    uint8_t microsteps,
                    float speed) 
    : AccelStepper(interface, step_pin, dir_pin),
    isRunning(false),
    isRunningClockwise(false),
    sps(speed),
    microstepping(microsteps),
    stepsPerFullRotation(defaultStepsSpecs * microstepping),
    steps90DegreesRotation(stepsPerFullRotation / 4)
    {
        this->setMaxSpeed(sps);
        this->setAcceleration(sps);
    };
    void StartRunClockwise();
    void StartRunAnticlockwise();
    void UpdateStepper();
    bool IsRunning();
    void setCurrentEnablePin(uint8_t pin);
};

