import { computed, toRef } from 'vue';

export function useFormat(data) {
  const birthday = toRef(data, 'birthday');

  const formattedBirthday = computed({
    get() {
      const date = new Date(birthday.value);
      const dateTimeFormat = new Intl.DateTimeFormat('en-CA');
      const string = dateTimeFormat.format(date);
      return string;
    },
    set(newValue) {
      birthday.value = new Date(newValue).toISOString();
    }
  });

  return { formattedBirthday };
}
