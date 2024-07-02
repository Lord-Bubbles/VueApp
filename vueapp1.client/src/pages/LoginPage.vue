<script setup>
  import { useAuthStore } from '@/stores/authStore';
  import { Field, Form, ErrorMessage } from 'vee-validate';
  import { object, string } from 'yup';

  const schema = object().shape({
    username: string().email('Please enter a valid email').required('Email is required'),
    password: string().required('Password is required')
  });

  const loginUser = async (values) => {
    const authStore = useAuthStore();
    await authStore.login(values);
  };
</script>

<template>
  <main class="center-content">
    <div class="grid-location card p-4">
      <h1 class="d-flex justify-content-center">Login</h1>
      <Form
        @submit="loginUser"
        :validation-schema="schema"
        :validate-on-mount="false"
        v-slot="{ isSubmitting }"
      >
        <div class="mb-3">
          <label class="form-label">Username</label>
          <Field class="form-control" type="email" placeholder="Email" name="username" />
          <ErrorMessage class="text-danger" name="username" />
        </div>
        <div class="mb-4">
          <label class="form-label">Password</label>
          <Field type="password" placeholder="Password" class="form-control" name="password" />
          <ErrorMessage class="text-danger" name="password" />
        </div>
        <button class="d-block mb-3 btn btn-info w-100 rounded-pill" type="submit">
          {{ isSubmitting ? 'Logging in. Please wait...' : 'Login' }}
        </button>
        <span>
          Don't have an account?
          <router-link :to="{ name: 'signup' }" class="link-info text-decoration-none">
            Create one here.
          </router-link>
        </span>
      </Form>
    </div>
  </main>
</template>
