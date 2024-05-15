<script setup>
  import { inject, ref } from 'vue';
  import { useRouter } from 'vue-router';

  const formData = ref({
    username: '',
    password: ''
  });

  const router = useRouter();
  const { getManagers } = inject('allManagers');

  const createUser = async () => {
    try {
      const response = await fetch('api/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          Accept: 'application/json'
        },
        body: JSON.stringify({
          username: formData.value.username,
          password: formData.value.password
        })
      });
      await response.json();
      await router.push({ name: 'login' });
      await getManagers();
    } catch (error) {
      alert('A user with this username already exists!');
      console.log('An error has occured while creating user: ' + error);
    }
  };
</script>

<template>
  <main>
    <h1 class="d-flex justify-content-center">Sign Up</h1>
    <form @submit.prevent="createUser">
      <div class="mb-3">
        <label class="form-label">Username</label>
        <input class="form-control" type="email" placeholder="Email" v-model="formData.username" />
      </div>
      <div class="mb-4">
        <label class="form-label">Password</label>
        <input
          type="password"
          class="form-control"
          placeholder="Password"
          autocomplete="new-password"
          v-model="formData.password"
        />
      </div>
      <button type="submit" class="btn btn-info w-100 mb-3 rounded-pill">Create Account</button>
      <span>
        Already have an account?
        <router-link :to="{ name: 'login' }" class="link-info text-decoration-none">
          Sign in here.
        </router-link>
      </span>
    </form>
  </main>
</template>
