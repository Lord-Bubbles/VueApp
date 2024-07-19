<script setup>
  import { useAuthStore } from '@/stores/authStore';
  import { Field, ErrorMessage, useForm } from 'vee-validate';
  import { useToast } from 'vue-toastification';
  import { object, string } from 'yup';

  const { isSubmitting, setFieldError, handleSubmit } = useForm({
    validationSchema: object().shape({
      username: string().email('Please enter a valid email').required('Email is required'),
      password: string().required('Password is required')
    }),
    validateOnMount: false
  });

  const loginUser = handleSubmit(async (values) => {
    const authStore = useAuthStore();
    const toast = useToast();
    try {
      await authStore.login(values);
    } catch (error) {
      if (error.message == 'Bad Request') {
        setFieldError('form', 'Incorrect username or password');
      } else {
        toast.error('An error has occurred while logging in');
      }
    }
  });
</script>

<template>
  <main class="center-content">
    <div class="grid-location card p-4">
      <h1 class="d-flex justify-content-center">Login</h1>
      <form @submit="loginUser">
        <div class="mb-3">
          <label class="form-label">Username</label>
          <Field class="form-control" type="email" placeholder="Email" name="username" />
          <ErrorMessage class="text-danger" name="username" />
        </div>
        <div class="mb-3">
          <label class="form-label">Password</label>
          <Field type="password" placeholder="Password" class="form-control" name="password" />
          <ErrorMessage class="text-danger" name="password" />
        </div>
        <ErrorMessage class="text-danger d-block mb-3" name="form" />
        <button class="d-block mb-3 btn btn-info w-100 rounded-pill" type="submit">
          {{ isSubmitting ? 'Logging in. Please wait...' : 'Login' }}
        </button>
        <span>
          Don't have an account?
          <router-link :to="{ name: 'signup' }" class="link-info text-decoration-none">
            Create one here.
          </router-link>
        </span>
      </form>
    </div>
  </main>
</template>
