<script setup>
  import { useQueryClient } from '@tanstack/vue-query';
  import { useRoute, useRouter } from 'vue-router';
  import AutoComplete from './AutoComplete.vue';

  const formData = defineModel({ type: Object, required: true });
  const router = useRouter();
  const route = useRoute();
  const queryClient = useQueryClient();

  const reset = () => {
    formData.value = {
      name: '',
      minAge: 0,
      maxAge: 0,
      managerName: '',
      type: '',
      email: '',
      page: 1,
      limit: formData.value.limit
    };
    router.push({ name: route.name, query: { page: 1, limit: route.query.limit } });
    queryClient.invalidateQueries({ queryKey: ['users'], refetchType: 'all' });
  };
</script>

<template>
  <div>
    <div class="input-group mb-3">
      <input
        type="text"
        class="form-control"
        placeholder="Search"
        :value="formData.name"
        @input="
          (e) => {
            formData.name = e.target.value;
            const query = Object.assign({}, route.query);
            if (!formData.name) {
              delete query.name;
            } else {
              query.name = e.target.value;
            }
            router.replace({
              query
            });
          }
        "
      />
      <span class="input-group-text"><i class="bi bi-search"></i></span>
      <div class="dropdown">
        <button
          type="button"
          class="ms-3 btn btn-secondary"
          data-bs-toggle="dropdown"
          data-bs-auto-close="false"
        >
          <i class="bi bi-list"></i>
        </button>
        <div class="dropdown-menu dropdown-menu-end p-4 text-bg-dark" style="width: 30rem">
          <div class="row mb-3">
            <div class="col">
              <label class="form-label">Min Age</label>
              <input
                type="number"
                class="form-control"
                :value="formData.minAge"
                @input="
                  (e) => {
                    formData.minAge = e.target.value;
                    const query = Object.assign({}, route.query);
                    if (formData.minAge <= 0) {
                      delete query.minAge;
                    } else {
                      query.minAge = e.target.value;
                    }
                    router.replace({
                      query
                    });
                  }
                "
                min="0"
                :max="formData.maxAge"
              />
            </div>
            <div class="col">
              <label class="form-label">Max Age</label>
              <input
                type="number"
                class="form-control"
                :value="formData.maxAge"
                @input="
                  (e) => {
                    formData.maxAge = e.target.value;
                    const query = Object.assign({}, route.query);
                    if (formData.maxAge <= 0) {
                      delete query.maxAge;
                    } else {
                      query.maxAge = e.target.value;
                    }
                    router.replace({
                      query
                    });
                  }
                "
                :min="formData.minAge"
                max="100"
              />
            </div>
          </div>
          <AutoComplete v-model="formData.managerName" />
          <div class="mb-3">
            <label class="form-label">Employee Type</label>
            <select
              class="form-select"
              :value="formData.type"
              @change="
                (e) => {
                  formData.type = e.target.value;
                  const query = Object.assign({}, route.query);
                  if (!formData.type) {
                    delete query.type;
                  } else {
                    query.type = e.target.value;
                  }
                  router.replace({
                    query
                  });
                }
              "
            >
              <option value="">Select...</option>
              <option>Admin</option>
              <option>Manager</option>
              <option>Employee</option>
            </select>
          </div>
          <div class="d-flex justify-content-center">
            <button type="button" class="btn btn-primary" @click="reset">Reset</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>