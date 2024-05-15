<script setup>
  import { useRoute, useRouter } from 'vue-router';
  import { inject, computed, ref } from 'vue';

  const formData = defineModel();
  const router = useRouter();
  const route = useRoute();
  const { managerNames } = inject('allManagers');
  const managerInput = ref(null);

  const reset = () => {
    formData.value = {
      name: '',
      minAge: 0,
      maxAge: 0,
      manager: '',
      type: '',
      email: '',
      page: 1,
      limit: 20
    };
    router.replace({ name: route.name });
  };

  const filteredManagers = computed(() => {
    const filter = managerNames.filter((n) =>
      n.toLowerCase().includes(formData.value.manager.toLowerCase())
    );
    return filter.length > 0 ? filter : ['No results'];
  });
</script>

<template>
  <main>
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
          <div class="mb-3">
            <label class="form-label">Email</label>
            <input type="text" class="form-control" v-model="formData.email" placeholder="Email" />
          </div>
          <div class="mb-3">
            <label class="form-label">Manager Name</label>
            <div class="dropdown">
              <input
                ref="managerInput"
                type="text"
                class="form-control"
                :value="formData.manager"
                @change="
                  (e) => {
                    formData.manager = e.target.value;
                    const query = Object.assign({}, route.query);
                    if (!formData.manager) {
                      delete query.manager;
                    } else {
                      query.manager = e.target.value;
                    }
                    router.replace({
                      query
                    });
                  }
                "
                placeholder="Manager Name"
                data-bs-toggle="dropdown"
              />
              <ul class="dropdown-menu">
                <li v-for="(name, index) in filteredManagers" :key="name + '-' + index">
                  <button
                    type="button"
                    class="dropdown-item"
                    :disabled="name === 'No results' ? true : false"
                    @click="() => (managerInput.value = name)"
                  >
                    {{ name }}
                  </button>
                </li>
              </ul>
            </div>
          </div>
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
              <option>admin</option>
              <option>manager</option>
              <option>employee</option>
            </select>
          </div>
          <div class="d-flex justify-content-center">
            <button type="button" class="btn btn-primary" @click="reset">Reset</button>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>