#include "stm32l4xx_hal.h"

void calibrate(float* gyro_offset){
	float xSum = 0, ySum = 0, zSum = 0;
	int sampleNum = 500;
	float gyroVal[3];

	for (int i = 0; i < sampleNum; i++){
		BSP_GYRO_GetXYZ(gyroVal);

		xSum += gyroVal[0];
		ySum += gyroVal[1];
		zSum += gyroVal[2];

		HAL_Delay(1);
	}

	gyro_offset[0] = xSum / sampleNum;
	gyro_offset[1] = ySum / sampleNum;
	gyro_offset[2] = zSum / sampleNum;
}
