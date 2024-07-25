<script setup>
  import { useAuthStore } from '@/stores/authStore';
  import { Field, ErrorMessage, useForm } from 'vee-validate';
  import { useToast } from 'vue-toastification';
  import { object, ref, string } from 'yup';

  const { setFieldError, isSubmitting, handleSubmit } = useForm({
    validationSchema: object().shape({
      email: string().email('Please enter a valid email').required('Email is required'),
      firstName: string().required('First name is required'),
      lastName: string().required('Last name is required'),
      password: string()
        .required('Password is required')
        .min(8, ({ min }) => `Password must be at least ${min} characters long`),
      confirmPassword: string().oneOf([ref('password')], 'Passwords must match')
    }),
    validateOnMount: false
  });

  const createUser = handleSubmit(async (values) => {
    delete values.confirmPassword;
    const authStore = useAuthStore();
    const toast = useToast();
    try {
      await authStore.register(values);
    } catch (error) {
      if (error.message == 'Bad Request') {
        setFieldError('email', 'A user with this email already exists');
      } else {
        toast.error('An unexpected error has occurred while registering');
      }
    }
  });
</script>

<template>
  <main class="center-content">
    <div class="card p-4 m-4">
      <h1 class="d-flex justify-content-center">Sign Up</h1>
      <form @submit="createUser">
        <div class="mb-3">
          <label class="form-label">Username</label>
          <Field class="form-control" type="email" placeholder="test@email.com" name="email" />
          <ErrorMessage class="text-danger" name="email" />
        </div>
        <div class="mb-3">
          <label class="form-label">First Name</label>
          <Field class="form-control" type="text" placeholder="John" name="firstName" />
          <ErrorMessage class="text-danger" name="firstName" />
        </div>
        <div class="mb-3">
          <label class="form-label">Last Name</label>
          <Field class="form-control" type="text" placeholder="Doe" name="lastName" />
          <ErrorMessage class="text-danger" name="lastName" />
        </div>
        <div class="mb-3">
          <label class="form-label">Password</label>
          <Field
            type="password"
            class="form-control"
            placeholder="Password"
            autocomplete="new-password"
            name="password"
          />
          <ErrorMessage class="text-danger" name="password" />
        </div>
        <div class="mb-3">
          <label class="form-label">Confirm Password</label>
          <Field
            type="password"
            class="form-control"
            placeholder="Confirm password"
            autocomplete="new-password"
            name="confirmPassword"
          />
          <ErrorMessage class="text-danger" name="confirmPassword" />
        </div>
        <button type="submit" class="btn btn-info w-100 mb-3 rounded-pill">
          {{ isSubmitting ? 'Submitting...' : 'Submit' }}
        </button>
        <span>
          Already have an account?
          <router-link :to="{ name: 'login' }" class="link-info text-decoration-none">
            Sign in here.
          </router-link>
        </span>
      </form>
    </div>
  </main>
</template>
