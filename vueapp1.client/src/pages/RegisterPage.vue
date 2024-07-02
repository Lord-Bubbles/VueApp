<script setup>
  import { useAuthStore } from '@/stores/authStore';
  import { Form, Field, ErrorMessage } from 'vee-validate';
  import { object, ref, string } from 'yup';

  const schema = object().shape({
    email: string().email('Please enter a valid email').required('Email is required'),
    firstName: string().required('First name is required'),
    lastName: string().required('Last name is required'),
    password: string()
      .required('Password is required')
      .min(8, 'Password must be at least 8 characters long'),
    confirmPassword: string().oneOf([ref('password')], 'Passwords must match!')
  });

  const createUser = async (values) => {
    delete values.confirmPassword;
    const authStore = useAuthStore();
    await authStore.register(values);
  };
</script>

<template>
  <main class="center-content">
    <div class="grid-location card p-4">
      <h1 class="d-flex justify-content-center">Sign Up</h1>
      <Form
        @submit="createUser"
        :validation-schema="schema"
        :validate-on-mount="false"
        v-slot="{ isSubmitting }"
      >
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
      </Form>
    </div>
  </main>
</template>
