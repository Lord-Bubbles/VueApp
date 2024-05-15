<script setup>
  import { ref, inject } from 'vue';
  import base64 from 'base-64';
  import { useRouter } from 'vue-router';

  const formData = ref({
    username: '',
    password: ''
  });

  const { updateUser } = inject('user');

  const router = useRouter();

  const loginUser = async () => {
    try {
      const response = await fetch('api/Login', {
        headers: {
          Authorization:
            'Basic ' + base64.encode(formData.value.username + ':' + formData.value.password),
          'Content-Type': 'application/json'
        }
      });
      const data = await response.json();
      updateUser(data);
      await router.push({ name: 'home', params: { id: data.id } });
    } catch (error) {
      alert('Incorrect username or password!');
      console.log('Error has occured while logging in ' + error);
    }
  };
</script>

<template>
  <main>
    <h1 class="d-flex justify-content-center">Login</h1>
    <form @submit.prevent="loginUser">
      <div class="mb-3">
        <label class="form-label">Username</label>
        <input class="form-control" type="email" placeholder="Email" v-model="formData.username" />
      </div>
      <div class="mb-4">
        <label class="form-label">Password</label>
        <input
          type="password"
          placeholder="Password"
          class="form-control"
          v-model="formData.password"
        />
      </div>
      <button class="d-block mb-3 btn btn-info w-100 rounded-pill" type="submit">Login</button>
      <span>
        Don't have an account?
        <router-link :to="{ name: 'signup' }" class="link-info text-decoration-none">
          Create one here.
        </router-link>
      </span>
    </form>
  </main>
</template>
