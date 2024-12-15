#include <stdio.h>
#include <fenv.h>

struct kstate {
    float q;
    float r;
    float x;
    float p;
    float k;
};

int kalmanFilter_update(struct kstate* kstate, float measurement){
	// Clear all floating-point exceptions before starting calculations
	feclearexcept(FE_ALL_EXCEPT);

	// Instantiate local variables that will hold the state variables
	float q = kstate->q;
	float r = kstate->r;
	float x = kstate->x;
	float k = kstate->k;
	float p = kstate->p;

	// Update estimated error covariance by adding process noise
	p = p + q;

	// Compute Kalman gain
	k = p / (p + r);

	// Update estimated state with correction factor
	x = x + k * (measurement - x);

	// Update error covariance
	p = (1 - k) * p;

	// Check for floating-point exceptions after all operations
	if (fetestexcept(FE_INVALID | FE_DIVBYZERO | FE_OVERFLOW | FE_UNDERFLOW)) {
		return -1;
	}

	// Store updated state variables
	kstate->q = q;
	kstate->r = r;
	kstate->x = x;
	kstate->k = k;
	kstate->p = p;

	return 0;
}
