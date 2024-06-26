#include <Arduino.h>
#include <string>
#include <AccelStepper.h>
#include <BluetoothSerial.h>
#include "PinsDefinitions.h"
#include "StepperController.h"

BluetoothSerial SerialBT;

bool stopRunningBtnPressed {false};
const unsigned long btnDebounceDelay = 150;
unsigned long lastStopRunningBtnDebounceTime = 0;
bool stopRunning {false};

float sps = 8000;    // Steps per second
uint8_t defaultStepsSpecs = 200;    // number of steps using full steps (stepper motor spec)
uint8_t microsteps = 16;    // driver microstepping 

StepperController StepperController1(AccelStepper::DRIVER, STEP1_PIN, DIR1_PIN, defaultStepsSpecs, microsteps, sps);
StepperController StepperController2(AccelStepper::DRIVER, STEP2_PIN, DIR2_PIN, defaultStepsSpecs, microsteps, sps);

std::string inputString;
 
void inline ResetEnables()
{
    digitalWrite(ENABLE_UP_PIN, HIGH); 
    digitalWrite(ENABLE_FRONT_PIN, HIGH);
    digitalWrite(ENABLE_RIGHT_PIN, HIGH);
    digitalWrite(ENABLE_DOWN_PIN, HIGH);
    digitalWrite(ENABLE_BACK_PIN, HIGH);
    digitalWrite(ENABLE_LEFT_PIN, HIGH);
}

void ProcessCharacter(const char &move)
{
    if(stopRunning)
        return;
    bool moveClockwise = islower(move);
    StepperController *controller;

    switch (tolower(move)) // rubik's cube moves notation: u, f, r, d, b, l, U, F, R, D, B, L
    {
        case 'u':
            StepperController1.setCurrentEnablePin(ENABLE_UP_PIN);
            controller = &StepperController1;
            break;
    
        case 'f':
            StepperController1.setCurrentEnablePin(ENABLE_FRONT_PIN);
            controller = &StepperController1;
            break;
    
        case 'r':
            StepperController1.setCurrentEnablePin(ENABLE_RIGHT_PIN);
            controller = &StepperController1;
            break;
    
        case 'd':
            StepperController2.setCurrentEnablePin(ENABLE_DOWN_PIN);
            controller = &StepperController2;
            break;
    
        case 'b':
            StepperController2.setCurrentEnablePin(ENABLE_BACK_PIN);
            controller = &StepperController2;
            break;
    
        case 'l':
            StepperController2.setCurrentEnablePin(ENABLE_LEFT_PIN);
            controller = &StepperController2;
            break;
    
        default:
            return; // exit if move is not valid
    }  

    if(stopRunning)
        return;
    
    if (moveClockwise)
        controller->StartRunClockwise();
    else
        controller->StartRunAnticlockwise();
}

void ProcessInputString(const std::string &str)
{
    for(uint16_t i = 0; i < str.length(); )
    {
        if(stopRunning)
            return;
        
        StepperController1.UpdateStepper();
        StepperController2.UpdateStepper();
        
        if (StepperController1.IsRunning() || StepperController2.IsRunning())
            continue;
        
        char move = str[i];
        bool moveClockwise = islower(move);
        
        if(i + 1 >= str.length())
        {
            ProcessCharacter(move);
            inputString.clear();
            break;
        }

        // verifies if it's possible to execute next move simultaneously
        char nextMove = str[i + 1];
        if ((tolower(move) == 'u' && tolower(nextMove) == 'd') ||
            (tolower(move) == 'd' && tolower(nextMove) == 'u') ||
            (tolower(move) == 'f' && tolower(nextMove) == 'b') ||
            (tolower(move) == 'b' && tolower(nextMove) == 'f') ||
            (tolower(move) == 'l' && tolower(nextMove) == 'r') ||
            (tolower(move) == 'r' && tolower(nextMove) == 'l'))
        {
            ProcessCharacter(move); 
            ProcessCharacter(nextMove); 
            i += 2;
        }
        else    // executes only one move
        {
            ProcessCharacter(move);
            i++;
        }
    }
}

void IRAM_ATTR StopRunning()
{
    stopRunning = !stopRunning;
    digitalWrite(LED_PIN, !stopRunning);
    stopRunningBtnPressed = false;
    inputString.clear();
    ResetEnables();
}

void setup()
{
    //Serial.begin(9600);
    SerialBT.begin("CubeSolver");
    pinMode(STOP_PIN, INPUT_PULLUP);
    pinMode(LED_PIN, OUTPUT);
    attachInterrupt(digitalPinToInterrupt(STOP_PIN), StopRunning, RISING);
    pinMode(ENABLE_UP_PIN, OUTPUT); 
    pinMode(ENABLE_FRONT_PIN, OUTPUT);
    pinMode(ENABLE_RIGHT_PIN, OUTPUT);
    pinMode(ENABLE_DOWN_PIN, OUTPUT);
    pinMode(ENABLE_BACK_PIN, OUTPUT);
    pinMode(ENABLE_LEFT_PIN, OUTPUT);
    ResetEnables();
    digitalWrite(LED_PIN, !stopRunning);
    delay(500);
}

void loop()
{
    if(stopRunning)
        return;
    if(SerialBT.available())
        inputString = SerialBT.readString().c_str();
    if(!inputString.empty())
        ProcessInputString(inputString);
}